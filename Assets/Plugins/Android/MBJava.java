package com.mb28.treeoflife;
import android.view.RoundedCorner;
import android.view.WindowInsets;
import android.app.Activity;
import android.os.Build;

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

    public static void ShowToast(Activity activity, String msg) {
    }

    

}