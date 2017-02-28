
import SOM as s
import pandas as pd
import sys

def main():

    args = sys.argv

    if len(args) != 2:
        print "<file_name>"
        exit(-1)

    data_loc = args[1]

    print data_loc
    data = pd.read_csv(data_loc, sep=',', header=1).as_matrix()

    print data.shape

    som = s.SelfOrganisingMap([data.shape[1], 20*20],20)
    som.Train(data,5000,0.5)



if __name__ == "__main__":
    main()