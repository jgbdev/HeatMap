# HeatMap

Hi! Thanks for trying out our Heat Map service. There are both Linux (Ubuntu or RaspberryPi) and Windows versions of the service.

# Windows

*Windows Download:* [http://tinyurl.com/cssheatmap](http://tinyurl.com/cssheatmap)

*Requirements:* Admin privileges, **Visual Studio** installed (sorry, we've only done a "dev" version so far)

*Installation instructions:*

1. Go to the download URL above
2. Click the "Download" arrow in the top-right corner to download the whole zip file
3. Unzip the file to a folder of your choice
4. Open the Visual Studio Developer Command Prompt (not an ordinary command prompt) (search in all programs)
5. "cd" to the unzipped folder
6. Run "installutil.exe heatmap_service.exe"
7. Open Services (Right click Windows button and click Services or open Task Manager, go to Services tab, then click "Open Services")
8. Scroll down the list to find "Heat Map Service", right click and click "Start"
9. If you want to check if the service is running properly, open the Windows Event Viewer and go to "Windows logs -> Applications" and look for any errors reported by "Heat Map Service"
10. Stop the service at some point. It's a manually controlled service so won't start/stop automatically.

# Linux

*Linux download:* Download the "RestfulClient.py" Python 2.7 script from the LinuxDeviceService in "master" branch of this repo.

**Note:** You must be running a recent version of **Python 2.7** not Python 3 for this to work.

## Ubuntu

1. Install the "requests" package: `pip install requests`
2. Install the "lm-sensors" package: `sudo apt-get install lm-sensors`
3. Setup lm-sensors: `sudo sensors-detect`
4. Verify your setup is working by running `sensors` (this should output some temperature data and stuff)
5. Run `python RestfulClient.py`
6. Enter your mode: "sensors" for Ubuntu
7. Let the magic happen!
8. Type "quit" then press enter to exit the program (don't use Ctrl+C or Ctrl+D - you'll have to use Task Manager or similar to kill Python if you use Ctrl+C/Ctrl+D)

## RaspberryPi

1. Install the "requests" package: `pip install requests`
2. Run `python RestfulClient.py`
3. Enter your mode: "rpi" for RaspberryPi
4. Let the magic happen!
5. Type "quit" then press enter to exit the program (don't use Ctrl+C or Ctrl+D - you'll have to use Task Manager or similar to kill Python if you use Ctrl+C/Ctrl+D)

# Thanks!

We hope this worked for you! Please let us know if it didn't.

Ross, John and Ed
