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
using SMTPd.Mime;
using SMTPd.Capabilities;
using System;


#endregion

using SMTPd.Smtp.Config;

namespace SMTPd.Smtp
{
    public class SmtpResponderFactory : ISmtpResponderFactory
    {
        public SmtpResponderFactory(IConfiguredSmtpRestrictions configuration,
            IEmailValidator emailValidator = null,
            IGetSmtpCapabilities getSmtpCapabilities = null)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            emailValidator = emailValidator ?? new XamarinEmailValidator();
            getSmtpCapabilities = getSmtpCapabilities ?? new GetDefaultSmtpCapabilities(configuration);

            DataResponder = new SmtpDataResponder(configuration);
            IdentificationResponder = new SmtpIdentificationResponder(configuration, getSmtpCapabilities);
            MailFromResponder = new SmtpMailFromResponder(configuration, emailValidator);
            RecipientToResponder = new SmtpRecipientToResponder(configuration, emailValidator);
            RawLineResponder = new SmtpRawLineResponder(configuration);
            ResetResponder = new SmtpResetResponder(configuration);
            VerifyResponder = new SmtpVerifyResponder(configuration);
        }

        public IRespondToSmtpData DataResponder { get; set; }

        public IRespondToSmtpIdentification IdentificationResponder { get; set; }

        public IRespondToSmtpMailFrom MailFromResponder { get; set; }

        public IRespondToSmtpRecipientTo RecipientToResponder { get; set; }

        public IRespondToSmtpRawLine RawLineResponder { get; set; }

        public IRespondToSmtpReset ResetResponder { get; set; }

        public IRespondToSmtpVerify VerifyResponder { get; set; }
    }
}
