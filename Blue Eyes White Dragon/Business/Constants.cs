﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue_Eyes_White_Dragon.Business
{
    public static class Constants
    {
        public static string GameImagesLocation => ConfigurationManager.AppSettings["GameImagesLocation"];
        public static string ReplacementImagesLocation => ConfigurationManager.AppSettings["ReplacementImagesLocation"];
        public enum SupportedImageTypes
        {
            jpg,
            png,
        }
    }
}