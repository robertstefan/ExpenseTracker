import api from '../emptySplitApi';

export const authEndpoints = api
  .enhanceEndpoints({
    addTagTypes: ['Auth'],
  })
  .injectEndpoints({
    endpoints: (builder) => ({
      loginUser: builder.mutation({
        query: (body) => ({
          url: 'auth/login',
          method: 'POST',
          body,
        }),
      }),
      registerUser: builder.mutation({
        query: (body) => ({
          url: 'auth/register',
          method: 'POST',
          body,
        }),
      }),
      completeResetPassword: builder.mutation({
        query: (params) => ({
          url: 'auth/reset-password',
          params,
        }),
      }),
      completeChangeEmail: builder.mutation({
        query: (params) => ({
          url: 'auth/change-email',
          params,
        }),
      }),
      unlockUser: builder.mutation({
        query: (id) => ({
          url: `auth/unlock/${id}`,
          method: 'POST',
        }),
      }),
    }),
  });

export const {
  useLoginUserMutation,
  useRegisterUserMutation,
  useCompleteResetPasswordMutation,
  useCompleteChangeEmailMutation,
  useUnlockUserMutation,
} = authEndpoints;
