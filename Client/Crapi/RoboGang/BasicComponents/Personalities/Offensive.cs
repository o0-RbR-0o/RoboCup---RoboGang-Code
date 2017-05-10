using System;
using System.Collections;
using System.Linq;
using TeamYaffa.CRaPI;
using TeamYaffa.CRaPI.Commands;
using TeamYaffa.CRaPI.Utility;
using TeamYaffa.CRaPI.World;
using TeamYaffa.CRaPI.World.GameObjects;

namespace RoboGang.RoboGang.BasicComponents.Personalities
{
    internal class Offensive : Personality
    {
        private bool _hasStartPosition;
        private bool _turned;

        // pass the ball to a passable mate
        public void DoPass(ArrayList passableMates)
        {
            var p = PlayerHandler.Context.Player;
            var longest = double.MinValue;
            var bestTaker = (FieldPlayer) passableMates[0];
            foreach (FieldPlayer mate in passableMates)
            {
                //check distance to all opponents
                var currentDistance = p.World.Opponents.Cast<FieldPlayer>()
                    .Sum(opponent => opponent.Position - mate.Position);

                if (!(currentDistance > longest) || mate.Goalie) continue;
                longest = currentDistance;
                bestTaker = mate;
            }

            p.CommandQueue.Enqueue(BasicActions.KickToPoint(p, bestTaker.Position, CalculateKickPower(8, bestTaker)));
            p.Send();
        }
			
        /*
        * Function: BlockOpponent
        * If stamina value > 1000 calculate intersection point 
        * with choosen opponent player and block them
        */

        public void BlockOpponent()
        {
            var p = PlayerHandler.Player;

            //if stamina < 1000
            if (p.SenseBody.Stamina > 1000)
            {
                Point2D intersectPoint;

                //get all blockable opponents in area
                var blockableOpps = GetBlockableOpps();

                //get closest opp in area
                var chosenOpponent = GetClosestOpponent(blockableOpps);

                //get intersection point
                MathUtility.IntersectionPoint(p, chosenOpponent, out intersectPoint);

                //get lookatpoint
                var lookPoint = chosenOpponent.Position.MidwayTo(p.World.TheBall.Position);

                p.CommandQueue.Enqueue(BasicActions.DashToPointLookingAtPoint(p, intersectPoint, (int) (p.ServerParam.MaxPower*0.9), lookPoint));
            }

            else
            {
                p.CommandQueue.Enqueue(BasicActions.TurnAndLookAtPoint(p, p.World.TheBall.Position));
            }

            p.Send();
        }


        /*
       * Fill with actions for each situation and return command to execute in the following code
       */

        //Before a kickoff
        public override Command DoBeforeKickOff()
        {
            var p = PlayerHandler.Context.Player;

            if (p.IsGoalie) return p.IsGoalie ? BasicCommands.Move(-50, 0) : null;

            if (_hasStartPosition) return null;

            _hasStartPosition = true;

            return BasicCommands.Move(p.World.TheBall.Position);
        }


        //While a kickoff when own team has to kick off.
        public override Command DoWhileKickOffOwn()
        {
            var p = PlayerHandler.Context.Player;

            if (p.BallIsKickable)
                //Random value for testing.
                return BasicCommands.Kick(40, 4);

            if (p.UniformNumber == 2 || p.UniformNumber == 3)
                return BasicCommands.Move(p.World.TheBall.Position);

            return BasicCommands.Move(p.World.TheBall.Position);
        }


        //While a kickoff when opponent team has to kick off.
        public override Command DoWhileKickOffOpponent()
        {
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }
            return p.World.TheBall.SeenThisCycle ? BasicActions.LookAtObject(p, p.World.TheBall) : BasicCommands.Turn(45);
        }


        //While play on
        public override Command DoWhilePlayOn()
        {
            var p = PlayerHandler.Context.Player;
            var rnd = new Random();
            if (p.World.TheBall.SeenThisCycle)
                if (p.BallIsKickable)
                {
                    var nearestTeammate = FindNearestTeammate();
                    if (nearestTeammate == null)
                        return BasicActions.KickToPoint(p, new Point2D(52, rnd.Next(-8, +8)), rnd.Next(180));

                    //Crapi is using double: I assume that is in radians? The server expects degrees, so i just converted that.
                    if (_turned)
                    {
                        if (rnd.Next(40)%40 != 0 && nearestTeammate.Goalie == false)

                            return BasicActions.KickToObject(p, nearestTeammate, (int) nearestTeammate.Distance*2);

                        return BasicActions.KickToPoint(p, new Point2D(52, rnd.Next(-8, +8)), rnd.Next(180));
                    }
                    _turned = true;
                    return BasicActions.TurnToObject(p, nearestTeammate);
                }
                else
                {
                    _turned = false;
                    if (!p.IsGoalie)
                        if (p.World.TheBall.Distance < 38)
                            if (FindNearestTeammate() != null)
                            {
                                if (FindNearestTeammate().Distance > 10 ||
                                    (FindNearestTeammate().Position.XDistanceTo(p.World.TheBall.Position) > 8 &&
                                     FindNearestTeammate().Position.YDistanceTo(p.World.TheBall.Position) > 8) ||
                                    p.World.TheBall.Distance < 3)
                                    return BasicActions.DashToPoint(p, p.World.TheBall.Position, 30*p.UniformNumber*5);
                                return BasicActions.DashToPoint(p, PlayerHandler.Context.Randpos, 50);
                            }
                            else
                                return BasicActions.DashToPoint(p, p.World.TheBall.Position, 30*p.UniformNumber*5);
                        else
                        {
                            return BasicActions.DashToPoint(p, PlayerHandler.Context.Randpos, 50);
                        }

                    if (p.BallIsCatchable)
                        return BasicActions.CatchBall(p);

                    if (Math.Abs(p.World.TheBall.Position.Y) < 15 && p.World.TheBall.Position.X < -35)
                        return BasicActions.DashToPoint(p, new Point2D(-50, p.World.TheBall.Position.Y), 300);

                    return p.World.TheBall.SeenThisCycle ? BasicActions.TurnToPoint(p, p.World.TheBall.Position) : BasicCommands.Turn(30);
                }

            if (p.IsGoalie) return BasicActions.TurnToPoint(p, p.World.TheBall.Position);

            return PlayerHandler.Runcycles%12 == 0 ? BasicActions.DashToPoint(p, PlayerHandler.Context.Randpos, 50) : BasicCommands.Turn(30);
        }


        public override Command DoWhileCornerOwn()
        {
            return null;
        }


        public override Command DoWhileCornerOpponent()
        {
            var p = PlayerHandler.Context.Player;

            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }

            return p.World.TheBall.SeenThisCycle ? BasicActions.LookAtObject(p, p.World.TheBall) : BasicCommands.Turn(45);
        }


        public override Command DoWhileFreekickOwn()
        {
            var p = PlayerHandler.Context.Player;

            if (p.BallIsKickable)
            {
                var staticObj = WorldModel.initStaticObjects(p.TeamSide);
                var goalMiddlePoint = p.TeamSide == Side.Left ? (Point2D) staticObj["g r"] : (Point2D) staticObj["g l"];
                if (p.World.Self.Position - goalMiddlePoint < 25)
                {
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
                }

                if (GetPlayerstoPass().Count <= 0)
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);

                DoPass(GetPlayerstoPass());
                return null;
            }

            if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle)
            {
                p.CommandQueue.Enqueue(BasicActions.DashToPointLookingAtPoint(p, p.World.TheBall.Position,
                    (int) (p.ServerParam.MaxPower*0.9), p.World.TheBall.Position));
                p.Send();
                return null;
            }

            if (FindPlayerwithBall() == null || FindPlayerwithBall().UniformNumber == p.UniformNumber)
                return p.World.TheBall.SeenThisCycle ? BasicActions.LookAtObject(p, p.World.TheBall) : BasicCommands.Turn(45);

            GetPassable(FindPlayerwithBall());
            return null;
        }


        public override Command DoWhileFreekickOpponent()
        {
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }

            return p.World.TheBall.SeenThisCycle ? BasicActions.LookAtObject(p, p.World.TheBall) : BasicCommands.Turn(45);
        }


        public override Command DoWhileGoalkickOwn()
        {
            var p = PlayerHandler.Context.Player;

            if (p.BallIsKickable)
            {
                var staticObj = WorldModel.initStaticObjects(p.TeamSide);
                var goalMiddlePoint = p.TeamSide == Side.Left ? (Point2D) staticObj["g r"] : (Point2D) staticObj["g l"];

                if (p.World.Self.Position - goalMiddlePoint < 25)
                {
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
                }
                if (GetPlayerstoPass().Count <= 0)
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
                DoPass(GetPlayerstoPass());
                return null;
            }

            if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle)
            {
                p.CommandQueue.Enqueue(BasicActions.DashToPointLookingAtPoint(p, p.World.TheBall.Position,
                    (int) (p.ServerParam.MaxPower*0.9), p.World.TheBall.Position));
                p.Send();
                return null;
            }
            if (FindPlayerwithBall() == null || FindPlayerwithBall().UniformNumber == p.UniformNumber)

                return p.World.TheBall.SeenThisCycle ? BasicActions.LookAtObject(p, p.World.TheBall) : BasicCommands.Turn(45);

            GetPassable(FindPlayerwithBall());
            return null;
        }


        public override Command DoWhileGoalkickOpponent()
        {
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }
            return p.World.TheBall.SeenThisCycle
                ? BasicActions.LookAtObject(p, p.World.TheBall)
                : BasicCommands.Turn(45);
        }


        public override Command DoAfterGoalOwn()
        {
            var p = PlayerHandler.Context.Player;
            return BasicCommands.Move(p.StartPoint);
        }


        public override Command DoAfterGoalOpponent()
        {
            var p = PlayerHandler.Context.Player;
            return BasicCommands.Move(p.StartPoint);
        }


        public override Command DoWhileKickInOwn()
        {
            var p = PlayerHandler.Context.Player;
            if (p.BallIsKickable)
            {
                var staticObj = WorldModel.initStaticObjects(p.TeamSide);
                var goalMiddlePoint = p.TeamSide == Side.Left ? (Point2D) staticObj["g r"] : (Point2D) staticObj["g l"];
                if (p.World.Self.Position - goalMiddlePoint < 25)
                {
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);
                }
                if (GetPlayerstoPass().Count <= 0)
                    return BasicActions.KickToPoint(p, goalMiddlePoint, p.ServerParam.MaxPower);

                DoPass(GetPlayerstoPass());
                return null;
            }

            if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle)
            {
                p.CommandQueue.Enqueue(BasicActions.DashToPointLookingAtPoint(p, p.World.TheBall.Position,
                    (int) (p.ServerParam.MaxPower*0.9), p.World.TheBall.Position));
                p.Send();
                return null;
            }

            if (FindPlayerwithBall() == null || FindPlayerwithBall().UniformNumber == p.UniformNumber)
                return p.World.TheBall.SeenThisCycle ? BasicActions.LookAtObject(p, p.World.TheBall) : BasicCommands.Turn(45);
            GetPassable(FindPlayerwithBall());
            return null;
        }


        public override Command DoWhileKickInOpponent()
        {
            var p = PlayerHandler.Context.Player;
            if (GetBlockableOpps().Count > 0 && GetClosestOpponent(GetBlockableOpps()) != null)
            {
                BlockOpponent();
            }
            return p.World.TheBall.SeenThisCycle ? BasicActions.LookAtObject(p, p.World.TheBall) : BasicCommands.Turn(45);
        }



        public override Command DoWhileUnknownPlaymode()
        {
            throw new NotImplementedException();
        }



        public override Command DoIfOffsideOwn()
        {
            var p = PlayerHandler.Context.Player;
            if (p.BallIsKickable)
            {
                if (GetPlayerstoPass().Count > 0)
                {
                    DoPass(GetPlayerstoPass());
                    return null;
                }
                if (FindNearestTeammate() != null)
                {
                    return BasicActions.KickToPoint(p, FindNearestTeammate().Position,
                        CalculateKickPower(8, FindNearestTeammate()));
                }
                if (FindTeammatefarbehind() != null)
                {
                    return BasicActions.KickToPoint(p, FindTeammatefarbehind().Position,
                        CalculateKickPower(10, FindTeammatefarbehind()));
                }
            }
            if (ClosestOfMatesToPosition(p.World.TheBall.Position) && p.World.TheBall.SeenThisCycle)
            {
                p.CommandQueue.Enqueue(BasicActions.DashToPointLookingAtPoint(p, p.World.TheBall.Position,
                    (int) (p.ServerParam.MaxPower*0.9), p.World.TheBall.Position));
                p.Send();
                return null;
            }
            if (FindPlayerwithBall() != null && FindPlayerwithBall().UniformNumber != p.UniformNumber)
            {
                GetPassable(FindPlayerwithBall());
                return null;
            }
            if (p.World.TheBall.SeenThisCycle)
            {
                return BasicActions.LookAtObject(p, p.World.TheBall);
            }
            return BasicCommands.Turn(45);
        }


        public override Command DoIfOffsideOpponent()
        {
            return null;
        }


        public override Command DoAfterTimeOut()
        {
            throw new NotImplementedException();
        }
    }
}