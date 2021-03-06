﻿using Strava.Activities;
using Strava.Athletes;
using Strava.Authentication;
using Strava.Clients;
using Strava.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StravaConsole
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Test();
            Console.ReadLine();
        }

        public async static void Test()
        {
            Console.WriteLine("Framework: {0}", Strava.Common.Framework.Version);

            // Please use your own, valid token!
            const String token = "75372e025e4089e2bd7927213fd0f4584279fc53";

            // Use either the static authentication method or use the WebAuthentication method.
            StaticAuthentication auth = new StaticAuthentication(token);

            //WebAuthentication auth = new WebAuthentication();
            //auth.AuthCodeReceived += delegate(object sender, AuthCodeReceivedEventArgs args) { Debug.WriteLine("Auth Code: " + args.AuthCode); };
            //auth.AccessTokenReceived += delegate(object sender, TokenReceivedEventArgs args) { Debug.WriteLine("Access Token: " + args.Token); };
            //auth.GetTokenAsync("<Client id>", "<Client Secret>", Scope.Full);

            // You can either use the StravaClient or 'single' clients like the ActivityClient.
            // The StravaClient offers predefined clients.
            StravaClient client = new StravaClient(auth);

            AthleteSummary a = await client.Athletes.GetAthleteAsync();
            Console.WriteLine("{0} {1}", a.FirstName, a.LastName);

            DateTime? LastActivity;
            using (StravaEntities context = new StravaEntities()) {
                LastActivity = (from v in context.Activities select v.TimeStarted).Max();               
            }
            if (!LastActivity.HasValue) {
                LastActivity = DateTime.Parse("2009-01-01");
            }

            #region Activities

            var activities = client.Activities.GetActivities(LastActivity.Value, DateTime.Now);
            
            using (StravaEntities context = new StravaEntities())
            {
                foreach (var item in activities)
                {
                    var act = (from v in context.Activities where v.ActivityId == item.Id select v).FirstOrDefault();
                    bool NewAct = false;
                    if (act == null)
                    {
                        act = new Activity();
                        NewAct = true;
                    }

                    act.ActivityId = (int)item.Id;
                    act.Name = item.Name;
                    act.ActivityType = item.Type.ToString();
                    act.AverageCadence = (decimal)item.AverageCadence;
                    act.AverageHeartRate = (decimal)item.AverageHeartrate;
                    act.TimeStarted = item.DateTimeStart;
                    //act.TimeEnded = item.DateTimeStart
                    //act.TimeEnded = item.
                    //act.TimeEnded = item.DateTimeStart + item.tim
                    act.ElevationGain = (decimal)item.ElevationGain;
                    act.ElapsedSeconds = item.ElapsedTime;
                    act.MovingSeconds = item.MovingTime;
                    act.TimeEnded = act.TimeStarted.Value.AddSeconds(act.ElapsedSeconds.Value);
                    act.Distance = (decimal)item.Distance;
                    act.MovingTime = item.MovingTimeSpan;
                    act.ElapsedTime = item.ElapsedTimeSpan;
                    var DistKm = (decimal)item.Distance / 1000;
                    if (DistKm != 0) {
                        act.AveragePace = new TimeSpan((long)(item.ElapsedTimeSpan.Ticks / DistKm));
                    }
                    
                    var gear = (from g in context.Gears where g.GearId == item.GearId select g).FirstOrDefault();

                    if (gear == null)
                    {
                        try
                        {
                            var SGear = client.Gear.GetGear(item.GearId);
                            Gear g = new Gear();
                            g.GearId = item.GearId;
                            g.Description = SGear.Name;
                            context.Gears.Add(g);
                            context.SaveChanges();

                        }
                        catch (Exception)
                        {
                            //Gear probaby deleted
                        }

                    }

                    act.GearId = item.GearId;

                    DateTime d;
                    if (DateTime.TryParse(item.StartDate, out d))
                    {
                        act.Date = d;
                    }

                    

                    if (NewAct)
                        context.Activities.Add(act);

                   

                    if (false) {
                        List<ActivityLap> laps = await client.Activities.GetActivityLapsAsync(act.ActivityId.ToString());

                        context.Laps.RemoveRange(context.Laps.Where(x => x.ActivityId == act.ActivityId));
                        context.SaveChanges();

                        foreach (var lap in laps)
                        {
                            Lap l = new Lap();
                            l.ActivityId = act.ActivityId;
                            l.LapId = lap.LapIndex;
                            l.Id = (int)lap.Id;
                            l.Start = lap.Start;
                            l.ElapsedTime = lap.ElapsedTime;
                            l.MovingTime = lap.MovingTime;
                            l.TotalElevationGain = (decimal)lap.TotalElevationGain;
                            //l.StartLocal = lap.StartLocal;
                            l.StartIndex = lap.StartIndex;
                            l.EndIndex = lap.EndIndex;
                            l.MaxHeartRate = (decimal)lap.MaxHeartrate;
                            l.MaxSpeed = (decimal)lap.MaxSpeed;
                            l.Distance = (decimal)lap.Distance;
                            context.Laps.Add(l);
                            context.SaveChanges();
                        }
                    }
                    
                    Console.WriteLine(DateConverter.ConvertIsoTimeToDateTime(item.StartDateLocal));
                }
                context.SaveChanges();

            }

            //Console.WriteLine("Async: " + activitiesAsync.Count);

            #endregion

            #region Athlete

            //Athlete current = await client.Athletes.GetAthleteAsync();
            //Console.WriteLine(current);

            //List<AthleteSummary> friends = await client.Athletes.GetFriendsAsync();
            //Console.WriteLine(friends.Count);

            //List<AthleteSummary> bothFollowing = await client.Athletes.GetBothFollowingAsync("528819");
            //Console.WriteLine(bothFollowing.Count);

            //List<SegmentEffort> records = await client.Segments.GetRecordsAsync("528819");

            //foreach (SegmentEffort effort in records)
            //{
            //    Console.WriteLine(effort.Name);
            //}

            //List<SegmentSummary> starred = await client.Segments.GetStarredSegmentsAsync();
            //Console.WriteLine(starred.Count);

            #region Update Athlete Weight

            //Athlete a = client.GetAthlete();
            //Console.WriteLine(a.FirstName + " " + a.LastName);

            //Athlete updated = client.UpdateAthlete(AthleteParameter.Weight, "60.0");
            //Console.WriteLine(updated.Email);

            #endregion

            #endregion

            #region Map Decoder

            //Activity mapActivity = await client.Activities.GetActivityAsync("109557593", false);
            //List<Coordinate> coords = PolylineDecoder.Decode(mapActivity.Map.Polyline);

            //foreach (var coordinate in coords)
            //{
            //    Console.WriteLine(coordinate);
            //}

            #endregion

            #region Leaderboard

            // Get the leaderboard of a segment.
            //Leaderboard leaderboard = client.Segments.GetFullSegmentLeaderboard("1300798");

            //foreach (LeaderboardEntry entry in leaderboard.Entries)
            //{
            //    Console.WriteLine(entry.ToString());
            //}

            #endregion

            #region Comments

            //List<Comment> comments = await client.Activities.GetCommentsAsync("112861810");

            //foreach (var comment in comments)
            //{
            //    Console.WriteLine(comment.Text);
            //    Console.WriteLine();
            //}

            #endregion

            #region Kudos

            //List<Athlete> kudoAthletes = await client.Activities.GetKudosAsync("112818941");

            //foreach (var kudos in kudoAthletes)
            //{
            //    Console.WriteLine(kudos.FirstName);
            //}

            #endregion

            #region Club

            //Athlete clubAthlete = await client.Athletes.GetAthleteAsync();

            //foreach (Club club in clubAthlete.Clubs)
            //{
            //    Console.WriteLine(club.Id);
            //}

            //Club c = await client.Clubs.GetClubAsync("949");
            //Console.WriteLine("Club: {0}", c.Name);

            //Image image = await ImageLoader.LoadImage(new Uri(c.Profile));
            //Form form = new Form();
            //form.Controls.Add(new PictureBox() { Dock = DockStyle.Fill, Image = image, SizeMode = PictureBoxSizeMode.CenterImage });
            //form.ShowDialog();

            //// Gets all the clubs of the currently authenticated user.
            //Console.WriteLine("Clubs:");
            //List<ClubSummary> clubs = await client.Clubs.GetClubsAsync();

            //foreach (var club in clubs)
            //{
            //    Console.WriteLine(club.Name);
            //}

            //// Only returns a result, if the athlete is a member in the club.
            //List<AthleteSummary> athletes = await client.Clubs.GetClubMembersAsync("949");
            //foreach (AthleteSummary athlete in athletes)
            //{
            //    Console.WriteLine(athlete.FirstName + " " + athlete.LastName);
            //}

            #endregion

            #region ActivityZones

            //List<ActivityZone> zones = client.Activities.GetActivityZones("114701243");

            //foreach (var item in zones.First().Buckets)
            //{
            //    Console.WriteLine(item.Maximum + " " + item.Minimum + " " + item.Time);
            //}

            #endregion

            #region Club Activities

            //List<ActivitySummary> items = client.Clubs.GetLatestClubActivities("949", 1, 5);

            //foreach (var activity in items)
            //{
            //    Console.WriteLine(activity.Name);
            //}

            //Console.WriteLine();

            //items = client.Clubs.GetLatestClubActivities("949", 2, 5);

            //foreach (var activity in items)
            //{
            //    Console.WriteLine(activity.Name);
            //}

            #endregion

            #region Latest activities of your friends

            //List<ActivitySummary> activities = await client.Activities.GetFriendsActivitiesAsync(10);

            //foreach (var item in activities)
            //{
            //    Console.WriteLine(item.Name);
            //}

            #endregion

            #region ActivityReceived Event-Test

            //client.Activities.ActivityReceived += delegate(object sender, ActivityReceivedEventArgs args) { Console.WriteLine(args.Activity.Name); };
            //await client.Activities.GetAllActivitiesAsync();

            #endregion

            #region Gear

            //Athlete athleteGear = client.Athletes.GetAthlete();

            //foreach (var bike in athleteGear.Bikes)
            //{
            //    Bike gear = await client.Gear.GetGearAsync(bike.Id);
            //    Console.WriteLine(gear.Name);
            //}

            //foreach (Shoes shoes in athleteGear.Shoes)
            //{
            //    Bike gear = await client.Gear.GetGearAsync(shoes.Id);
            //    Console.WriteLine(gear.Name);
            //}

            #endregion

            #region Streams

            //List<ActivityStream> streams = client.Streams.GetActivityStream("117700501", StreamType.LatLng | StreamType.Heartrate);
            //foreach (var stream in streams)
            //{
            //    Console.WriteLine(stream.StreamType);
            //    Console.WriteLine(stream.OriginalSize);
            //    Console.WriteLine(stream.Data.Count);
            //}


            //List<SegmentStream> segmentStreams = client.Streams.GetSegmentStream("6295188", SegmentStreamType.LatLng | SegmentStreamType.Distance, StreamResolution.All);
            //foreach (var stream in segmentStreams)
            //{
            //    Console.WriteLine(stream.StreamType);
            //    Console.WriteLine(stream.OriginalSize);
            //}

            //List<ActivitySummary> ac = client.Activities.GetActivitiesAfter(new DateTime(2014, 1, 1));
            //Console.WriteLine(ac.Count);

            #endregion

            #region Update Activity

            //Activity a = await client.Activities.UpdateActivityTypeAsync("<activity id>", ActivityType.Ride);
            //Console.WriteLine(a.Type);

            //Activity a = await client.Activities.UpdateActivityAsync("<activity id>", ActivityParameter.Name, "Kurze Testfahrt nach Schaltzug- und Kettenwechsel");
            //Console.WriteLine(a.Name);

            #endregion

            #region Weekly Progress

            //WeeklyProgress p = client.Activities.GetWeeklyProgress();

            //Console.WriteLine(p.TotalTime);
            //Console.WriteLine(p.RideDistance / 1000F);
            //Console.WriteLine(p.ActivityCount);

            #endregion

            #region Summary

            //var activities = client.Activities.GetSummary(new DateTime(2014, 1, 1), DateTime.Now);
            //var activities = client.Activities.GetSummaryLastYear();
            //var activities = client.Activities.GetSummaryThisYear();

            //Console.WriteLine("Activities: " + activities.ActivityCount);
            //Console.WriteLine("Rides: " + activities.Rides.Count);
            //Console.WriteLine("Ride Distance: " + activities.RideDistance / 1000D);

            #endregion

            #region Time Filter

            //Leaderboard l = await client.Segments.GetSegmentLeaderboardAsync("6861720", TimeFilter.ThisYear);
            //foreach (var s in l.Entries)
            //{
            //    Console.WriteLine(s.AthleteName);
            //}

            #endregion

            #region Segment-Explorer

            //ExplorerResult result = await client.Segments.ExploreSegmentsAsync(
            //    new Coordinate(37.821362, -122.505373),
            //    new Coordinate(37.842038, -122.465977),
            //    0,
            //    5);

            //foreach (ExplorerSegment s in result.Results)
            //{
            //    Console.WriteLine(s.Name);
            //}

            #endregion

            #region Segment Efforts

            //List<SegmentEffort> efforts = await client.Efforts.GetSegmentEffortsByTimeAsync("6386278", new DateTime(2014, 1, 1), new DateTime(2014, 4, 10));
            //List<SegmentEffort> efforts = await client.Efforts.GetSegmentEffortsByAthleteAsync("6386278", "1985994");
            //List<SegmentEffort> efforts = await client.Efforts.GetSegmentEffortsAsync("6386278", "1985994", new DateTime(2014, 4, 8), new DateTime(2014, 4, 10));
            //List<SegmentEffort> efforts = await client.Efforts.GetSegmentEffortsAsync("6386278");

            //foreach (SegmentEffort effort in efforts)
            //{
            //    Console.WriteLine(effort.Athlete.Id);
            //    Console.WriteLine(" " + effort.MovingTime / 60 + "m" + effort.MovingTime % 60 + "s");
            //}


            #endregion

            #region MapLoader

            //Activity a = await client.Activities.GetActivityAsync("129042840", false);
            //Debug.WriteLine(a.Map.SummaryPolyline);

            //System.Drawing.Image image = await GoogleImageLoader.LoadActivityPreviewAsync(a.Map.SummaryPolyline, new Dimension(150, 150), MapType.Terrain);
            //Form form = new Form();
            //form.Size = new Size(300, 300);

            //PictureBox p = new PictureBox();
            //p.Dock = DockStyle.Fill;
            //p.Image = image;
            //form.Controls.Add(p);

            //form.ShowDialog();

            #endregion

            #region Photos

            ////List<Photo> photos = await client.Activities.GetPhotosAsync("133317643");
            ////Debug.WriteLine("Photos: " + photos.Count);

            //List<Photo> photos = await client.Activities.GetLatestPhotosAsync(new DateTime(2014, 1, 1));
            //Debug.WriteLine("Photos: " + photos.Count);

            #endregion

            #region Laps

            //List<ActivityLap> laps = await client.Activities.GetActivityLapsAsync("135450490");

            //foreach (var lap in laps)
            //{
            //    Console.WriteLine((lap.Distance / 1000F).ToString("F"));
            //}

            #endregion
        }

        // convert datetime to unix epoch seconds
        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }
    }
}
