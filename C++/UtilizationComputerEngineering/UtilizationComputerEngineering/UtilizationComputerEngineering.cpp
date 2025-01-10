#include <stdio.h>
#include <stdlib.h>

#define BYTE unsigned char //그레이스케일의 값을 저장하는 범위는 unsigned char를 BYTE단위로 활용한다.
#define MAX_BYTE 255 //unsigned char의 최대값은 255이다.

//raw이미지파일에 각 픽셀의 값을 255/배열의크기로하여 각 배열의 값을 채워넣어 생성하낟.
void CreateLowImage(const char* filename, int width, int height);
//raw이미지파일을 읽어와 각 2차원 픽셀의 정보를 좌표와함께 값을 출력한다.
void PrintLowImage(const char* filename, int width, int height);

//생성된 이미지파일을 테스트하고 결과를 확인한다.
void LowImageTestMain()
{
	CreateLowImage("5x5.raw", 5, 5);
	PrintLowImage("5x5.raw", 5, 5);

	PrintLowImage("genphotoshop.raw", 5, 5);
}

void main()
{
	LowImageTestMain();
}

void CreateLowImage(const char* filename, int width, int height)
{
	FILE* pFile = NULL;
	fopen_s(&pFile, filename, "wb");

	if (pFile == NULL) printf("file create failed! %s\n", filename);
	int nBufferSize = width * height;
	BYTE* imageBuffer = (BYTE*)malloc(nBufferSize);

	BYTE incByte = (int)MAX_BYTE / nBufferSize;
	printf("CreateLowImage(%s,%d,%d) incByte:%d\n", filename, width, height, incByte);
	BYTE writeByte = 0;
	for (int i = 0; i < nBufferSize; i++)
	{
		writeByte += incByte;
		imageBuffer[i] = writeByte;
	}

	fwrite((void*)imageBuffer, nBufferSize, nBufferSize, pFile);
	fclose(pFile);

	delete[] imageBuffer;
}

void PrintLowImage(const char* filename, int width, int height)
{
	FILE* pFile = NULL;

	fopen_s(&pFile, filename, "rb");

	if (pFile == NULL) printf("file read failed! %s\n", filename);
	printf("PrintLowImage(%s,%d,%d)\n", filename, width, height);
	int nBufferSize = width * height;
	BYTE* imageBuffer = (BYTE*)malloc(nBufferSize);

	fread(imageBuffer, nBufferSize, nBufferSize, pFile);

	for (int y = 0; y < height; y++)
	{
		for (int x = 0; x < width; x++)
		{
			int idx = width * y + x;
			printf("%d[%d,%d]:%d, ", idx, x, y, imageBuffer[idx]);
		}
		printf("\n");
	}

	fclose(pFile);
	delete[] imageBuffer;
}
