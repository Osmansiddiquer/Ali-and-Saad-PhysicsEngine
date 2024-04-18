using GameEngine.src.physics.body;

namespace GameEngine.src.physics.component;
public interface Component
{
    void RunComponent(RigidBody2D body, double delta);
}

