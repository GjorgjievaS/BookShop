﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain
{
    public class StripeSettings
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
