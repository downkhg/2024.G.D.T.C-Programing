#include <stdio.h>
#include <stdlib.h>

#define BYTE unsigned char //�׷��̽������� ���� �����ϴ� ������ unsigned char�� BYTE������ Ȱ���Ѵ�.
#define MAX_BYTE 255 //unsigned char�� �ִ밪�� 255�̴�.

//raw�̹������Ͽ� �� �ȼ��� ���� 255/�迭��ũ����Ͽ� �� �迭�� ���� ä���־� �����ϳ�.
void CreateLowImage(const char* filename, int width, int height);
//raw�̹��������� �о�� �� 2���� �ȼ��� ������ ��ǥ���Բ� ���� ����Ѵ�.
void PrintLowImage(const char* filename, int width, int height);

//������ �̹��������� �׽�Ʈ�ϰ� ����� Ȯ���Ѵ�.
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
