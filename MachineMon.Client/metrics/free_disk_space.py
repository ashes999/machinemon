import ctypes
import os
import platform
import sys

class FreeDiskSpaceMetric:
    def get_metric(self):
        percent_free = self.get_free_space_percent()
        return { "message": "{0}% free".format(percent_free), "is_error": percent_free <= 25 }

    def get_free_space_percent(self):
        """Return the percentage of free space on the main drive, as an integer (eg. 48%)"""
        return 100 * self.get_free_space_gb() // self.get_total_space_gb()

    def get_free_space_gb(self):
        """Return free space (in gigabytes) on the main drive."""
        if platform.system() == 'Windows':
            drive_name = "C:"
            free_bytes = ctypes.c_ulonglong(0)
            ctypes.windll.kernel32.GetDiskFreeSpaceExW(ctypes.c_wchar_p(drive_name), None, None, ctypes.pointer(free_bytes))
            return free_bytes.value / 1024 / 1024 / 1024
        else:
            drive_name = "/var/"
            st = os.statvfs(drive_name)
            return st.f_bavail * st.f_frsize / 1024 / 1024 / 1024

    def get_total_space_gb(self):
        """Return drive total space (in gigabytes) on the main drive."""
        if platform.system() == 'Windows':
            drive_name = "C:"
            total_bytes = ctypes.c_ulonglong(0)
            ctypes.windll.kernel32.GetDiskFreeSpaceExW(ctypes.c_wchar_p(drive_name), None, ctypes.pointer(total_bytes), None)
            return total_bytes.value / 1024 / 1024 / 1024
        else:
            drive_name = "/var/"
            st = os.statvfs(drive_name)
            return st.f_bsize * st.f_blocks / 1024 / 1024 / 1024