#include <iostream>
#include <omp.h>
#include <cmath>
#include "hw1.h"


double euclidean_length(std::vector<double> vector) {
  // Your code goes here.
  double total = 0.0;
  #pragma opm parallel for reduction(+:total)
  for(int n=0; n<vector.size(); n++) {
    total += vector[n]*vector[n];
  }
  return sqrt(total);
}


void up_sweep(std::vector<int> &indexes) {
  int n = indexes.size();
  int nceil = (int) std::pow(2, ceil(std::log2(n)));
  int h = (int) std::log2(nceil);

  for(int i=1; i<=h; i++) {
    int step = 1 << i;
    #pragma omp parallel for
    for(int j=step-1; j<n; j+=step) {
      indexes[j] += indexes[j - step/2];
    }
  }
}


void down_sweep(std::vector<int> &indexes) {
  int n = indexes.size();
  int nceil = (int) std::pow(2, ceil(std::log2(n)));
  int h = (int) std::log2(nceil);
  
  for(int i=h-1; i > 0; i--) {
    int step = 1 << i;
    #pragma omp parallel for
    for(int j=step - 1; j<n-step/2; j += step) {
      indexes[j + step/2] += indexes[j];
    }
  }
}


std::vector<int64_t> discard_duplicates(std::vector<int64_t> sorted_vector) {
  // Your code goes here
  std::vector<int> indexes = std::vector<int>(sorted_vector.size(), 0);
  int i, n, val, d, max, h, temp;
  n = sorted_vector.size();

  std::cout << "INPUT:                     ";
  for (auto i = sorted_vector.begin(); i != sorted_vector.end(); ++i)
    std::cout << *i << ' ';
  std::cout << std::endl;
  
  // find when the numbers change. give 1 = new, 0=duplicate of previous value
  indexes[0] = 1;
  #pragma opm parallel for default(shared) private(i)
  for(i=1; i<n; i++) {
    if(sorted_vector[i] != sorted_vector[i-1])
      indexes[i] = 1;
  }

  std::cout << "T/F UNIQUE ONLY RESULTS:   ";
  for (auto i = indexes.begin(); i != indexes.end(); ++i)
    std::cout << *i << ' ';
  std::cout << std::endl;

  
// SERIAL:                                           
//  for(i=1; i<n; i++)
//    indexes[i] = indexes[i] + indexes[i-1];


  up_sweep(indexes);
  std::cout << "AFTER UPWARD SWEEP:        ";
  for (auto i = indexes.begin(); i != indexes.end(); ++i)
     std::cout << *i << ' ';
  std::cout << std::endl;


  down_sweep(indexes);
  std::cout << "AFTER DOWNWARD SWEEP:      ";
  for (auto i = indexes.begin(); i != indexes.end(); ++i)
     std::cout << *i << ' ';
  std::cout << std::endl;  


  max = indexes[n-1];
  std::vector<int64_t> unique = std::vector<int64_t>(max);
  #pragma opm parallel for default(shared) private(i, val)
  for(i=0; i<n; i++) {
    val = indexes[i] - 1;
    unique[val] = sorted_vector[i];
  }
 
  return unique;
}

