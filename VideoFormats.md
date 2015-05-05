# Live video formats #

This plugin is requesting live video in either [MPEG-4](http://en.wikipedia.org/wiki/MPEG-4) or [H.264](http://en.wikipedia.org/wiki/H.264/MPEG-4_AVC). The format used is decided based on camera firmware version. All Axis network cameras with firmware version 5.0 or newer are able to produce H.264 video, while older cameras produce MPEG-4.

The plugin itself does not install any MPEG-4 or H.264 decoders, but chances are you've already installed them. [MediaPortal](http://www.team-mediaportal.com/) is after all an application used primary for watching videos of various formats. If you by some chance haven't got the decoders installed, it is always possible to do so by visiting the camera's web page.