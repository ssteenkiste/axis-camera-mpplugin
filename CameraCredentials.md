# Camera Credentials #

When adding a camera to the plugin, the user is requested to specify the camera credentials. There are three reasons for that:

  * The user will get an early verification that entered credentials are valid
  * The plugin can determine the camera capabilities by requesting camera parameters that only camera administrators have access to
  * The credentials are remembered by the plugin, and when the user is requesting a live video stream from a camera inside MediaPortal, the saved credentials are used, thus not forcing the user to specify credentials inside MediaPortal

The third reason implies that if a user is requested to specify the credentials inside MediaPortal, the credentials on the camera have changed. Edit the camera and enter the new credentials.