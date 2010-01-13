/*  Copyright (C) 2009 Jos van der Til
    
    This file is part of the Fluent DataAnnotations Library.

    The Fluent DataAnnotations Library is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    The Fluent Metadata Library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with the Fluent DataAnnotations Library.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace FluentDataAnnotations
{
    public class DataAnnotationsTypeDescriptionProvider : TypeDescriptionProvider
    {
        private readonly Assembly _metadataAssembly;
        private static readonly DataAnnotationsCache Cache = new DataAnnotationsCache();
        public const bool CACHING = true;

        public DataAnnotationsTypeDescriptionProvider(TypeDescriptionProvider existingProvider)
            : this(existingProvider, null)
        {
        }

        public DataAnnotationsTypeDescriptionProvider(TypeDescriptionProvider existingProvider, Assembly metaDataAssembly)
            : base(existingProvider)
        {
            _metadataAssembly = metaDataAssembly;
        }

        public static void PrimeCacheFromAssembly(Assembly assembly)
        {
            var types = from a in assembly.GetTypes()
                        where a.BaseType != null && a.BaseType.IsSubclassOf(typeof(DataAnnotations))
                        select a;

            foreach(var type in types)
            {
                if(type.GetGenericArguments().Count() == 1)
                {
                    var objectType = type.GetGenericArguments()[0];

                    var annotations = assembly.CreateInstance(type.FullName) as DataAnnotations;

                    Cache.AddToCache(objectType, annotations);
                }
            }
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            var assembly = (_metadataAssembly ?? objectType.Assembly);
            //select all classes that inherit from fluentmetadata
            // Example: ProductMetadata -> FluentMetadata<Product> -> FluentMetadata
            DataAnnotations annotations;

            if (CACHING && Cache.IsInCache(objectType))
            {
                annotations = Cache.RetreiveFromCache(objectType);
            }
            else
            {
                var types = from a in assembly.GetTypes()
                            where a.BaseType != null && a.BaseType.IsSubclassOf(typeof (DataAnnotations))
                            select a;

                //select all classes that have the specified objectType as generic parameter.
                var type = (from a in types
                            where a.BaseType.GetGenericArguments().Contains(objectType)
                            select a).SingleOrDefault();

                //instantiate the metadata type
                if (type == null)
                {
                    //No FluentMetadata found for type, try searching in global?
                    //Returning base for now.
                    return base.GetTypeDescriptor(objectType, instance);
                }

                annotations = assembly.CreateInstance(type.FullName) as DataAnnotations;

                Cache.AddToCache(objectType, annotations);
            }

            //process it.
            return new DataAnnotationsPropertyDescriptor(annotations.Annotations, base.GetTypeDescriptor(objectType, instance));
        }
    }

    internal class DataAnnotationsCache
    {
        public IDictionary<Type, DataAnnotations> Cache = new Dictionary<Type, DataAnnotations>();
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public bool IsInCache(Type type)
        {
            if(type == null)
                return false;

            _lock.EnterReadLock();

            try
            {
                return Cache.ContainsKey(type);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public DataAnnotations RetreiveFromCache(Type type)
        {
            _lock.EnterReadLock();

            try
            {
                return Cache[type];
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void AddToCache(Type type, DataAnnotations annotations)
        {
            _lock.EnterWriteLock();

            Cache[type] = annotations;

            _lock.ExitWriteLock();
        }
    }
}