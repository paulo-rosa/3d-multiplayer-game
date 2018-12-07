public interface ICarController
{
    void Accelerate();
    void Turn();
    float CalculateTurn();
    void Jump();
    void SetIsFalling(bool value);
}