package com.mb28.treeoflife;
import android.view.RoundedCorner;
import android.view.WindowInsets;
import android.app.Activity;
import android.os.Build;
import android.content.Intent;
import android.net.Uri;
import android.provider.Settings;

public class MBJava {

    public static int cornerRadius(Activity activity) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.S) {
            WindowInsets insets = activity.getWindow().getDecorView().getRootWindowInsets();
            if (insets != null) {
                RoundedCorner corner = insets.getRoundedCorner(RoundedCorner.POSITION_TOP_LEFT);
                if (corner != null) {
                    return corner.getRadius();
                }
            }
        }
        return 0;
    }

    public static boolean canManageMedia() {
        return android.os.Environment.isExternalStorageManager();
    }

    public static void GoToAllFilesAccess(Activity activity) {
        if (canManageMedia() == false) {
            Intent intent = new Intent(Settings.ACTION_MANAGE_ALL_FILES_ACCESS_PERMISSION);
            intent.setData(Uri.fromParts("package", activity.getPackageName(), null));
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
            activity.startActivity(intent);
        }
    }

    public static void ShowToast(Activity activity, String msg) {
        android.widget.Toast.makeText(activity, msg, 0);
    }

}