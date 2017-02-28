ver=$(python --version 2>&1 | sed 's/.* \([0-9]\).\([0-9]\).*/\1\2/')
if [ "$ver" -eq "27" ]; then
  echo "Running Python 2.7.X - Yay! :)";
  
  wget https://raw.githubusercontent.com/jgbdev/HeatMap/master/LinuxDeviceService/RestfulClient.py -O HeatMapService.py
  python HeatMapService.py
else
  echo "Not running Python 2.7.X - Boo... :("
  echo "Aborting installation."
fi