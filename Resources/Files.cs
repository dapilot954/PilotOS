using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotOS.Resources
{
    public static class Files
    {
        [ManifestResourceStream(ResourceName = "PilotOS.Resources.Wallpapers.image.bmp")] public static byte[] PilotOSBackroundRaw;
        [ManifestResourceStream(ResourceName = "PilotOS.Resources.Cursors.cursor1.bmp")] public static byte[] PilotOSCursorRaw;
        [ManifestResourceStream(ResourceName = "PilotOS.Resources.FileIcons.FolderIcon.bmp")] public static byte[] PilotOSFolderRaw;
        [ManifestResourceStream(ResourceName = "PilotOS.Resources.FileIcons.FileIcon.bmp")] public static byte[] PilotOSFileRaw;
        [ManifestResourceStream(ResourceName = "PilotOS.Resources.FileIcons.BackIcon.bmp")] public static byte[] PilotOSBackRaw;
        [ManifestResourceStream(ResourceName = "PilotOS.Resources.FileIcons.AddIcon.bmp")] public static byte[] PilotOSAddRaw;
        [ManifestResourceStream(ResourceName = "PilotOS.Resources.FileIcons.DeleteIcon.bmp")] public static byte[] PilotOSDeleteRaw;
    }
}
