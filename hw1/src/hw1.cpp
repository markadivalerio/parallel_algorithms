#include <iostream>
#include <omp.h>
#include <cmath>
#include "hw1.h"

// Mark DiValerio - mad2799
// Sanjaikanth Pillai - se9727
// EE 382V - Parallel Algorithms
// Summer 2020
// HW 1

double euclidean_length(std::vector<double> vector) {
  // Your code goes here.
  double total = 0.0;
  #pragma opm parallel for reduction(+:total)
  for(int n=0; n<vector.size(); n++) {
    total += vector[n]*vector[n];
  }
  return sqrt(total);
}

/*void parallel_prefix_try1(std::vector<int> &arr) {
  int n = arr.size();
  int h = (int) ceil(std::log2(n));

  // upward sweep
  for(int i=1; i<=h; i++) {
    int step = 1 << i;
    #pragma omp parallel for
    for(int j=step-1; j<n; j+=step) {
      arr[j] += arr[j - step/2];
    }
  }

  std::cout << "AFTER UPWARD SWEEP:        ";  
  for (auto i = arr.begin(); i != arr.end(); ++i)
    std::cout << *i << ' ';
  std::cout << std::endl;


  // downward sweep 
  for(int i=h-1; i > 0; i--) {
    int step = 1 << i;
    #pragma omp parallel for
    for(int j=step - 1; j<n-step/2; j += step) {
      arr[j + step/2] += arr[j];
    }
  }
  
  std::cout << "AFTER DOWNWARD SWEEP:      ";
  for (auto i = arr.begin(); i != arr.end(); ++i)
    std::cout << *i << ' ';
  std::cout << std::endl;
  
}*/

void print(std::vector<int64_t> arr) {
  for(int i=0; i < arr.size(); i++)
    std::cout << arr[i] << " ";
  std::cout << std::endl;
}


std::vector<int64_t> discard_duplicates(std::vector<int64_t> sorted_vector) {
  // Your code goes here
  int i, n, val, max;
  n = sorted_vector.size();
  double logn = std::log2(n); //assumes power of 2
  std::vector<int64_t> idx = std::vector<int64_t>(n, 0);

  std::cout << "INPUT:    ";
  print(sorted_vector);
  
  // find when the numbers change. give 1 = new, 0=duplicate of previous value
  idx[0] = 1;
  #pragma opm parallel for default(shared) private(i)
  for(i=1; i<n; i++) {
    if(sorted_vector[i] != sorted_vector[i-1])
      idx[i] = 1;
  }

  std::cout << "UNIQUE:   ";
  print(idx);

  std::cout << "------PARALLEL PREFIX DEBUG------" << std::endl;
  std::cout << "UPWARD:" << std::endl;
  std::cout << "logn = " << logn << std::endl;
  for(int h=0; h < logn; h++) {
    int twoh = std::pow(2,h);
    int twoh1 = std::pow(2, h+1);
    std::cout << "h=" << h << "   2^h=" << twoh << "   2^(h+1)=" << twoh1 << std::endl;

    #pragma omp parallel for private(i)
    for(i=0; i<n-1; i += twoh1) {
      idx[i + twoh1 - 1] = idx[i + twoh - 1] + idx[i + twoh1 - 1];
    }
    #pragma omp barrier
    print(idx);
  }

  std::cout << std::endl << "DOWNWARD:" << std::endl;

  idx[n-1] = 0;
  for(int h=logn-1; h >= 0; h--) {
    int twoh = std::pow(2,h);
    int twoh1 = std::pow(2, h+1);
    std::cout << "h=" << h << "   2^h=" << twoh << "   2^(h+1)=" << twoh1 << std::endl;
    #pragma omp parallel for private(i, val)
    for(i=0; i < n; i+=twoh1) {
      val = idx[i + twoh - 1];
      idx[i + twoh - 1] = idx[i + twoh1 - 1];
      idx[i + twoh1 - 1] = val + idx[i + twoh1 - 1];
    }
    #pragma omp barrier
    print(idx);
  }
  std::cout << "-----END PARALLEL PREFIX-----" << std::endl;

  max = idx[n-1];
  #pragma opm parallel for private(i, val)
  for(i=1; i<n; i++) {
    val = sorted_vector[i];
    sorted_vector[idx[i]] = val;
  }
  #pragma omp barrier
  
  if(max < n)
    sorted_vector.erase(sorted_vector.begin() + max, sorted_vector.end());
  return sorted_vector;

}

