all: libtextoolwrap.so

clean:
	rm -f textoolwrap.o
	rm -f libtextoolwrap.so

textoolwrap.o: textoolwrap.cpp
	$(CXX) -c -fpic -o textoolwrap.o textoolwrap.cpp

libtextoolwrap.so: textoolwrap.o
	$(CXX) -shared -o libtextoolwrap.so textoolwrap.o -LPVRTexLib/Linux_x86_64 -lPVRTexLib -LIspc/linux64 -lispc_texcomp -LCrunch/linux64 -lcrnlib -Wl,-rpath,"\$$ORIGIN"