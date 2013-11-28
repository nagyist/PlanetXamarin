﻿/*
 * MvxSettings:
 * Copyright (C) 2013 Ceton Corp: 
 * 
 * Contributors:
 * http://github.com/JamesMontemagno
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 */

using System;
using CrossPlatformInVS.Portable.Interfaces;
using MonoTouch.Foundation;

namespace CrossPlatformInVS.Touch.PlatformSpecific
{
    public class TouchSettings : ISettings
    {

        private readonly object m_Locker = new object();

        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T)) where T : IComparable
        {
            lock (m_Locker)
            {
                Type typeOf = typeof (T);
                if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof (Nullable<>))
                {
                    typeOf = Nullable.GetUnderlyingType(typeOf);
                }
                object value = null;
                var typeCode = Type.GetTypeCode(typeOf);
                var defaults = NSUserDefaults.StandardUserDefaults;
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        value = defaults.BoolForKey(key);
                        break;
                    case TypeCode.Int64:
                    case TypeCode.Double:
                        value = defaults.DoubleForKey(key);
                        break;
                    case TypeCode.String:
                        value = defaults.StringForKey(key);
                        break;
                    case TypeCode.Int32:
                        value = defaults.IntForKey(key);
                        break;
                    case TypeCode.Single:
                        value = defaults.FloatForKey(key);
                        break;
                }
          

                return null != value ? (T)value : defaultValue;
            }
        }

        /// <summary>
        /// Adds or updates the value 
        /// </summary>
        /// <param name="key">Key for settting</param>
        /// <param name="value">Value to set</param>
        /// <returns>True of was added or updated and you need to save it.</returns>
        public bool AddOrUpdateValue(string key, object value)
        {
            lock (m_Locker)
            {
                Type typeOf = value.GetType();
                if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    typeOf = Nullable.GetUnderlyingType(typeOf);
                }
                var typeCode = Type.GetTypeCode(typeOf);
                var defaults = NSUserDefaults.StandardUserDefaults;
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        defaults.SetBool(Convert.ToBoolean(value), key);
                        break;
                    case TypeCode.Int64:
                    case TypeCode.Double:
                        defaults.SetDouble(Convert.ToDouble(value), key);
                        break;
                    case TypeCode.String:
                        defaults.SetString(Convert.ToString(value), key);
                        break;
                    case TypeCode.Int32:
                        defaults.SetInt(Convert.ToInt32(value), key);
                        break;
                    case TypeCode.Single:
                        defaults.SetFloat(Convert.ToSingle(value), key);
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Saves all currents settings outs.
        /// </summary>
        public void Save()
        {
            try
            {
                lock (m_Locker)
                {
                    var defaults = NSUserDefaults.StandardUserDefaults;
                    defaults.Synchronize();
                }
            }
            catch (Exception)
            {
                //TODO: log stuff here
            }
        }

    }

}