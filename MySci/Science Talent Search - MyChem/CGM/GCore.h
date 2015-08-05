#include <vector>
using namespace std;
struct Vector
{
	double X;
	double Y;
};
class Particle
{
public:
	Vector pos;
};
class GParticle : public Particle
{
public:
	Vector vel;
	double mass;
};
extern "C"
{
class GNewtonSimulator
{
	double timeEllapsed;
	vector<GParticle> particleList;
public:
	
	__declspec(dllexport) double GetTimeEllapsed();
	__declspec(dllexport) void AddParticle(const GParticle pTemp);
	__declspec(dllexport) void AddParticle(const double mass, const double X, const double Y, const double velX, const double velY);
	__declspec(dllexport) void IncreaseTime(const double timePast);
	
};
}