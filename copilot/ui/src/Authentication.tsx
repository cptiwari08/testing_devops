import { ReactNode, FC, useCallback, useState, useEffect, useMemo } from 'react';
import { useIsAuthenticated, useMsal } from '@azure/msal-react';
import Loader from './common/components/Loader/Loader';
import { useAppDispatch, useAppSelector } from './modules/hooks/hooks';
import { fetchCeToken } from './modules/redux/slices/userAuth';
import config from './common/configs/env.config';
import { SourceType } from './common/utility/constants';
import Unauthorized from './common/components/Unauthorized/Unauthorized';
import SkeletonLoader from './common/components/SkeletonLoader/SkeletonLoader';

type AuthenticationProps = {
  children: ReactNode;
};

const Authentication: FC<AuthenticationProps> = ({ children }) => {
  const dispatch = useAppDispatch();
  const isAuthenticated = useIsAuthenticated();
  const { inProgress } = useMsal();
  const projectId = useMemo(() => config.PROJECT_ID, []);
  const scopes: SourceType[] = useMemo(() => Object.values(SourceType), []);
  const isStrictAudience = useMemo(() => true, []);
  const { token, error, loading: tokenLoading } = useAppSelector((state: any) => state.userAuth);
  const [isLoading, setIsLoading] = useState(true);

  const fetchToken = useCallback(() => {
    if (isLoading && isAuthenticated && inProgress === 'none') {
      dispatch(fetchCeToken({ projectId, scopes, isStrictAudience }));
    }
  }, [isLoading, isAuthenticated, inProgress, projectId, scopes, isStrictAudience, dispatch]);

  useEffect(() => {
    fetchToken();
  }, [fetchToken]);

  useEffect(() => {
    if (token || error) {
      setIsLoading(tokenLoading);
    }
  }, [tokenLoading, token, error]);

  if (isLoading) {
    return window.location.pathname.includes("/configuration")? <Loader isLoading={true} />: <SkeletonLoader />;
  } else if (error) {
    return <Unauthorized />;
  } else {
    return <>{children}</>;
  }
};

export default Authentication;
