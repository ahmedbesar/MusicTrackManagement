import { Track } from './track.model';
import { TrackDistribution } from './track-distribution.model';

export interface TrackDetail extends Track {
  distributions: TrackDistribution[];
}
