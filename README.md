# NetQD
.NET port of the QD library implementing the double-double and quad-double technique for achieving almost 128-bit and 256-bit floating point precision types.

See [original paper](http://web.mit.edu/tabbott/Public/quaddouble-debian/qd-2.3.4-old/docs/qd.pdf) by David H. Bailey Yozo Hida and Xiaoye S. Li for mathematical details. An unofficial copy of the original C++/Fortran implementation is available e.g. [here](https://github.com/aoki-t/QD).

Note that the port is in it's early stages so there may be some bugs.

[![Build Status](https://dev.azure.com/rzikmund/NetQD/_apis/build/status/rzikm.NetQD?branchName=master)](https://dev.azure.com/rzikmund/NetQD/_build/latest?definitionId=1&branchName=master)

# Installing

> dotnet add package NetQD

# Roadmap

- Comprehensive set of unit tests that check that nothing has been broken during porting
- Add more math functions (Exp, Log, Pow...)
- Fit dotnet IFormatProvider into parsing and printing code

# Contributing

Pull Requests are welcome.
