import { DistributionStatus } from './distribution-status.model';

export interface TrackDistribution {
  id: string;
  dspId: string;
  dspName: string;
  submittedAt: string;
  status: DistributionStatus;
}
