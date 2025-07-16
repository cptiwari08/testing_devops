import moment from 'moment';
import { UserAuthState, fetchCeTokenSuccess } from '../../modules/redux/slices/userAuth';
import { getCeToken } from '../../modules/redux/apis/sspApis';
import config from '../configs/env.config';
import { store } from '../../modules/redux/core/store';
import { SourceType } from './constants';

const projectId = config.PROJECT_ID;
const GRACE_PERIOD_SECONDS = 300;
const REFRESH_TOKEN_PERIOD = 60 * 1000;

export const refreshToken = async () => {
    try {
        const scopes: SourceType[] = Object.values(SourceType);
        const isStrictAudience = true;
        const token = await getCeToken({ projectId, scopes, isStrictAudience });
        if (token) {
            store.dispatch(fetchCeTokenSuccess(token));
        }
        return token;
    } catch (error) {
        console.error('Error refreshing token:', error);
        throw error;
    }
};

export const refreshTokenIfNeeded = async () => {
    const { userAuth }: { userAuth: UserAuthState } = store.getState();
    if (userAuth.token && userAuth.userAuthDetails.exp) {
        const currentTime = moment.utc().unix();
        const expiresSoon = currentTime > (userAuth.userAuthDetails.exp - GRACE_PERIOD_SECONDS);
        if (expiresSoon) {
            await refreshToken();
        }
    }
};

export const startTokenMonitor = () => {
    let intervalId: NodeJS.Timeout | null = setInterval(async () => {
        try {
            await refreshTokenIfNeeded();
        } catch (error) {
            console.error('Monitoring stopped due to token refresh error');
            if (intervalId) {
                clearInterval(intervalId);
                intervalId = null;
            }
        }
    }, REFRESH_TOKEN_PERIOD);

    return () => {
        if (intervalId) {
            clearInterval(intervalId);
            intervalId = null;
        }
    };
};


