// GravityModule.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "GCore.h"
double GNewtonSimulator::GetTimeEllapsed()
{
	return timeEllapsed;
}
void GNewtonSimulator::IncreaseTime(const double timePast)
{
	timeEllapsed += timePast;
	for(int i = 0; i < particleList.size(); i++)
	{ 
		for (int o = i + 1; o < particleList.size(); o++)
		{
			double angle;
			double dX = particleList[i].pos.X - particleList[o].pos.X;
			double dY = particleList[i].pos.Y - particleList[o].pos.Y;
			angle = atan2(dY,dX);
			double rSqr = dX * dX + dY * dY; // Finds distance in pixels
			if (rSqr < 1600) { rSqr = 1600; }
			//double r = Math.Sqrt(rSqr);
			//// Change in Ek
			//double dEk = (-(constants[i][(o - 1) - i] / r) + (constants[i][(o - 1) - i] / oldR[i][(o - 1) - i]))/*Now finds the distance in metres*/;

			//double dYEk = Math.Sin(angle) * dEk;
			//double dXEk = Math.Cos(angle) * dEkjerewee
			double force = particleList[i].mass  / (rSqr);
			particleList[i].vel.Y -= sin(angle) * force / particleList[i].mass;
			particleList[o].vel.Y += sin(angle) * force / particleList[o].mass;
			particleList[i].vel.X -= cos(angle) * force / particleList[i].mass;
			particleList[o].vel.X += cos(angle) * force / particleList[o].mass;

			//oldR[i][(o - 1) - i] = r;
		}
	}
}