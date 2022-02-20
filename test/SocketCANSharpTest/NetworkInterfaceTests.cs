#region License
/* 
BSD 3-Clause License

Copyright (c) 2021, Derek Will
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its
   contributors may be used to endorse or promote products derived from
   this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/
#endregion

using NUnit.Framework;
using SocketCANSharp;
using SocketCANSharp.Network;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace SocketCANSharpTest
{
    public class ListCanDevicesTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TearDown]
        public void Cleanup()
        {
        }

        [Test]
        public void GetListOfNetworkInterfaces_Success_Test()
        {
            IntPtr ptr = LibcNativeMethods.IfNameIndex();
            Assert.AreNotEqual(IntPtr.Zero, ptr);

            try
            {
                IntPtr iteratorPtr = ptr;
                IfNameIndex i = Marshal.PtrToStructure<IfNameIndex>(ptr);
                while (i.Index != 0 && i.Name != null)
                {
                    Console.WriteLine(i);
                    iteratorPtr = IntPtr.Add(iteratorPtr, Marshal.SizeOf(typeof(IfNameIndex)));
                    i = Marshal.PtrToStructure<IfNameIndex>(iteratorPtr);
                }
            }
            finally
            {
                LibcNativeMethods.IfFreeNameIndex(ptr);
            }
        }

        [Test]
        public void GetListOfCanNetworkInterfaces_Success_Test()
        {
            IEnumerable<CanNetworkInterface> collection = CanNetworkInterface.GetAllInterfaces(true);
            Assert.IsNotNull(collection);
            Assert.GreaterOrEqual(collection.Count(), 1);
            foreach (var canInterface in collection)
            {
                Console.WriteLine(canInterface);
                Assert.Greater(canInterface.Index, 0);
                Assert.IsNotNull(canInterface.Name);
            }
        }
    }
}