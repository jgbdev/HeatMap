if [[ $(python --version 2>&1) == *2\.7* ]]; then
  echo "Running Python 2.7.X - Yay! :)";
  
  wget https://raw.githubusercontent.com/jgbdev/HeatMap/master/LinuxDeviceService/RestfulClient.py -O HeatMapService.py
  echo "rpi
  quit
  " | python HeatMapService.py
else
  echo "Not running Python 2.7.X - Boo... :("
  echo "Aborting installation."
fi