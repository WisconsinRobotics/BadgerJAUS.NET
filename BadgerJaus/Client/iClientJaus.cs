using System;

public interface iClientJaus
{
    bool setSpeedAndDirection(double speed, double direction);
    bool setArmPosition(double percentExtend, double angleLeft, double angleUp);
}