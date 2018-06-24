//-----------------------------------------------------------------------
// <copyright file="NancyRequestHelper.cs">
//     Copyright (c) 2016-2018 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

namespace ChannelAdam.Nancy
{
    using System;
    using System.IO;

    using global::Nancy;

    public static class NancyRequestHelper
    {
        #region Public Methods

        public static string GetRequestBodyAsString(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            string result = string.Empty;
            MemoryStream ms = null;

            if (request.Body.Position != 0)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            try
            {
                ms = ReadIntoMemoryStream(request);

                using (var reader = new StreamReader(ms))
                {
                    ms = null;
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                ms?.Dispose();
            }

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private static MemoryStream ReadIntoMemoryStream(Request request)
        {
            int currentByte;
            MemoryStream ms = new MemoryStream();

            while ((currentByte = request.Body.ReadByte()) != -1)
            {
                ms.WriteByte((byte)currentByte);
            }

            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        #endregion Private Methods
    }
}