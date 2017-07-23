#include <vector>
#include <math.h>
using namespace std;

struct Vector2
{
	double X;
	double Y;
};
class Particle
{
};
class StandardParticle :public Particle
{
public:
	Vector2 vel;
	Vector2 pos;
	double mass;
};
class GParticle :public StandardParticle
{
};
class GNewtonSimulator
{

	vector<GParticle> particleList;
	double timeEllapsed = 0;
public: 
	double GetTimeEllapsed();
	void AddGParticle(const GParticle gParticle);
	void IncreaseTime(const double timePast);
};
