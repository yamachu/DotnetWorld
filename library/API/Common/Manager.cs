using System;
using System.Collections.Generic;
using DotnetWorld.Utils;

namespace DotnetWorld.API.Common
{
    public static class Manager
    {
        private static ICore CoreAPI;
        private static ITools ToolsAPI;

        static Manager()
        {
            switch (PlatformDepend.Platform)
            {
                case PlatformDepend.EnumPlatform.LINUX:
                    CoreAPI = (ICore)new Linux.Core();
                    ToolsAPI = (ITools)new Linux.Tools();
                    break;
                case PlatformDepend.EnumPlatform.OSX:
                    CoreAPI = (ICore)new OSX.Core();
                    ToolsAPI = (ITools)new OSX.Tools();
                    break;
                case PlatformDepend.EnumPlatform.WINDOWS:
                    CoreAPI = (ICore)new Windows.Core();
                    ToolsAPI = (ITools)new Windows.Tools();
                    break;
            }
        }

        public static ICore GetWorldCoreAPI()
            => CoreAPI;

        public static ITools GetWorldToolsAPI()
            => ToolsAPI;
    }

}