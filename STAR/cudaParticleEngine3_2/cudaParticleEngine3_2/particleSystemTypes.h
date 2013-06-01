#include "xnaTypes.h"

struct Particle
{
		unsigned char alpha;
        struct Vector2 pos;
        struct Vector2 velocity;
        float lifetime;
		float Rotation;
		struct Rectangle rect;
};

enum GravityType
{
    OverallForce =0,
    Point= 1,
    Newton= 2
};

enum CollisionType
{ 
    Collision=0,
    None=0
};

struct NewtonMass
{
    struct Vector2 center;
    float weight;
};

struct ParticleOptions
{
	float totalLifeTime;
	float elapsedGameTime;
	GravityType gravityType;
	Vector2 gravity;
	NewtonMass *mass;
	int newtonMassSize;
	CollisionType collisionType;
	struct Rectangle *collisionRects;
	int collisionRectSize;
	int size;
	float AirFriction;
	float Friction;
};

struct ArrayHolder
{
	Vector2 *device_positions,*device_velocities;
	struct Rectangle *device_rects,*device_collisionrects;
	float *device_lifetimes,*device_Rotations;
	unsigned char* device_alphas;
	int numberColRects;
};