import math as m
import numpy as np
import sys


class SelfOrganisingMap(object):
    def __init__(self, sizes , width):
        # TODO: Handle rectangle
        self.num_layers = len(sizes)
        self.sizes = sizes
        self.weights = [(1 / m.sqrt(x)) * np.random.randn(y, x)
                        for x, y in zip(sizes[:-1], sizes[1:])]

        self.x = width
        self.y = width
        self.radius = width/2

    def Train(self, data, iterations, learning_orig):

        data_shape = data.shape

        for iter in range(0, iterations):

            data_count = data_shape[0]

            m_time_constant = iterations / m.log(self.radius)

            m_iter_radius = self.radius * m.exp(-iter / m_time_constant)

            learning_rate = learning_orig * m.exp( - iter / m_time_constant)

            for d in range(0, data_count):

                lowest_distance = sys.float_info.max
                lowest_index = (-1,-1)



                for y in range(0, self.y ):
                    for x in range(0, self.x):
                        index = y*self.x + x
                        temp_distance = self.Cost(data[index], self.weights[0][index])
                        if temp_distance < lowest_distance:
                            lowest_distance = temp_distance
                            lowest_index = (x,y)

                m_iter_radius_square = m.pow(m_iter_radius,2)

                if lowest_index ==  (-1,-1):
                    print "Could not find a closest item"
                    exit(-1)

                for y in range(0, self.y ):
                    for x in range(0, self.x):
                        dist = m.pow(x-lowest_index[0],2) + m.pow(y-lowest_index[1],2)
                        if dist < m_iter_radius_square:
                            self.weights[0][index] += learning_rate * (data[index] - self.weights[0][index])







    def Cost(self, item, w):
        if len(item) != len(w):
            print "Len mismatch!"
            exit(-1)
        total = 0.0
        for i in range(0, len(item)):
            total += m.pow(item[i] - w[i], 2)

        total = m.sqrt(total)
        return total
