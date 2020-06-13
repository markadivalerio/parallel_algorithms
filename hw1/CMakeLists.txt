cmake_minimum_required(VERSION 3.6)
project(hw1)

set(CMAKE_CXX_STANDARD 14)

# FIND OPENMP
find_package(OpenMP REQUIRED)

set(SOURCE_FILES src/hw1.cpp src/main.cpp)
add_executable(hw1 ${SOURCE_FILES})
