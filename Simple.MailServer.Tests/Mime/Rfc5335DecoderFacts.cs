﻿#region Header
// Copyright (c) 2013 Hans Wolff
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using Simple.MailServer.Mime;
using Xunit;
using Xunit.Extensions;

namespace Simple.MailServer.Tests.Mime
{
    public class Rfc5335DecoderFacts
    {
        [Fact]
        public void decode_null()
        {
            Assert.Null(Rfc5335Decoder.Decode(null));
        }

        [Fact]
        public void decode_empty_string()
        {
            Assert.Empty(Rfc5335Decoder.Decode(""));
        }

        [Fact]
        public void decode_empty_quotedprintable_string()
        {
            Assert.Empty(Rfc5335Decoder.Decode("=?ISO-8859-1?Q??="));
        }

        [Fact]
        public void decode_empty_base64_string()
        {
            Assert.Empty(Rfc5335Decoder.Decode("=?ISO-8859-1?B??="));
        }

        [Fact]
        public void ignore_plain_text()
        {
            Assert.Equal("some plain text", Rfc5335Decoder.Decode("some plain text"));
        }

        [Fact]
        public void decode_single_utf8_quotedprintable()
        {
            Assert.Equal("RE: Frühling", Rfc5335Decoder.Decode("=?UTF-8?Q?RE=3A_Fr=C3=BChling?="));
        }

        [Fact]
        public void decode_single_utf8_base64()
        {
            Assert.Equal("test", Rfc5335Decoder.Decode("=?UTF-8?B?dGVzdA==?="));
        }

        [Theory]
        [InlineData("=?ISO-8859-1?Q?a?= =?ISO-8859-1?Q?b?=", "ab")]
        [InlineData("=?ISO-8859-1?Q?a?=  =?ISO-8859-1?Q?b?=", "ab")]
        [InlineData("=?ISO-8859-1?Q?a?=\r\n  =?ISO-8859-1?Q?b?=", "ab")]
        public void remove_whitespace_between_encoded_words(string encoded, string decoded)
        {
            Assert.Equal(decoded, Rfc5335Decoder.Decode(encoded));
        }

        [Theory]
        [InlineData("=?ISO-8859-1?Q?a?= b", "a b")]
        [InlineData("=?ISO-8859-1?Q?a?=  b", "a b")]
        public void keep_single_space_between_encoded_word_and_plain_word(string encoded, string decoded)
        {
            Assert.Equal(decoded, Rfc5335Decoder.Decode(encoded));
        }
    }
}
