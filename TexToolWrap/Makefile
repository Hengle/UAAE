all: libTexToolWrap.so

clean:
	rm -f textoolwrap.o
	rm -f libTexToolWrap.so

TexToolWrap.o: textoolwrap.cpp
	$(CXX) -c -fpic -o textoolwrap.o textoolwrap.cpp

libTexToolWrap.so: textoolwrap.o
	$(CXX) -shared -o libTexToolWrap.so TexToolWrap.o -LPVRTexLib/Linux_x86_64 -lPVRTexLib -Lispc/linux64 -lispc_texcomp -Wl,-rpath,"\$$ORIGIN"