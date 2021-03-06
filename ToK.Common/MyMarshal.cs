﻿using System;
using System.Runtime.InteropServices;

namespace ToK.Common
{
    public static class MyMarshal
    {
        /// <summary>
        /// Marshals a raw buffer to a given marshalable struct.
        /// </summary>
        public static unsafe T GetStructure<T>(byte[] buffer, int offset = 0) where T : struct
        {
            fixed (byte* bufferPin = &buffer[offset])
            {
                return GetStructure<T>(bufferPin);
            }
        }

        public static unsafe T GetStructure<T>(CCompoundBuffer buffer) where T : struct
        {
            return GetStructure<T>(buffer.RawBuffer, buffer.Offset);
        }

        public static unsafe T GetStructure<T>(byte* buffer) where T : struct
        {
            return (T)Marshal.PtrToStructure(new IntPtr(buffer), typeof(T));
        }

        /// <summary>
        /// Marshals a given T instance into a raw buffer.
        /// </summary>
        public static unsafe byte[] GetBytes<T>(T obj) where T : struct
        {
            byte[] rawBuffer = new byte[Marshal.SizeOf(obj)];

            fixed (byte* rawBufferPin = rawBuffer)
            {
                Marshal.StructureToPtr<T>(obj, new IntPtr(rawBufferPin), false);
            }

            return rawBuffer;
        }

        /// <summary>
        /// Crates a read-to-use marshaled instance of T
        /// </summary>
        /// <typeparam name="T">Type to be marshaled as zero-initialized instance.</typeparam>
        /// <returns>A zero-initialized instance of T.</returns>
        public static unsafe T CreateEmpty<T>() where T : struct
        {
            int typeSize = Marshal.SizeOf(typeof(T));

            byte* rawBuffer = stackalloc byte[typeSize];

            for (int i = 0; i < typeSize; i++)
                rawBuffer[i] = 0;
            
            T zeroInited = (T)Marshal.PtrToStructure(new IntPtr(rawBuffer), typeof(T));

            return zeroInited;
        }
    }
}