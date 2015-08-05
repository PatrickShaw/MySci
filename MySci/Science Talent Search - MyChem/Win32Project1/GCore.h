#include <vector>
using namespace std;
struct Vector
{
	double X;
	double Y;
};
class Particle
{public:
	Vector pos;
};
class GParticle : public Particle
{public:
	Vector vel;
	double mass;
};
class GNewtonSimulator
{
	double timeEllapsed;
	vector<GParticle> particleList;
public:
	double GetTimeEllapsed();
	void IncreaseTime(const double timePast);
};