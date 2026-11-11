## Telemetry logs
Info about output, Call locations, and purpose
   
   | Event | Parameters (type) | Fires when | Question it answers |
   |-------|-------------------|------------|---------------------|
   | `session_start` | `app_version` (string), `device_model` (string), `android` (string) | first frame of Boot | which build and device produced this log |
   | `level_start` | `level_id` (string), `attempt` (int), `coins` (int) | new run starts | the attempt number starting from 1; entry wealth |
   | `level_gameOver` | `level_id`, `time_s` (float), `coins_earned` (int), `score` (int) | on player crash, flip, or death | run time; score as distance; and total coins earned |
   | `upgrade_purchased` | `upgrade_id` (id), `level` (int), `cost` (int), `coins_left` (int) | inside Wallet `TryUpgrade` if successful, before Changed event | when the first upgrade lands; spend pacing |


This data is pushed to logcat at runtime (readable through ADB), and is output to `/storage/emulated/<userid>/Android/data/com.ryanJackman.coolMobileGame/files/telemetry.log`. 

None of this data leaves the device, as stated in the [data privacy statement](../CA1/privacy-statement.md)
