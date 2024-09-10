import api from '../emptySplitApi';

export const userEndpoints = api.injectEndpoints({
  endpoints: (builder) => ({
    getUsers: builder.query({
      query: (params) => ({
        url: '/users/list',
        params,
      }),
      transformResponse: (res) => res,
      providesTags: (result) =>
        result
          ? [
              ...result.items.map(({ id }) => ({ type: 'User', id })),
              { type: 'User', id: 'LIST' },
            ]
          : [{ type: 'User', id: 'LIST' }],
    }),
    
    getUser: builder.query({
      query: (id) => ({
        url: `/users/${id}`,
      }),
      transformResponse: (res) => res,
      providesTags: (result, error, id) => [{ type: 'User', id }],
    }),
    
    getUserTransactions: builder.query({
      query: (params) => ({
        url: `/users/${params.id}/transactions`,
        params,
      }),
      transformResponse: (res) => res,
      providesTags: (result) =>
        result
          ? [
              ...result.items.map(({ id }) => ({ type: 'UserTransactions', id })),
              { type: 'UserTransactions', id: 'USER-TRANSACTIONS' },
            ]
          : [{ type: 'UserTransactions', id: 'USER-TRANSACTIONS' }],
    }),

    updateUser: builder.mutation({
      query: ({ id, body }) => ({
        url: `users/update/${id}`,
        method: 'POST',
        body,
      }),
	  invalidatesTags: (_result, _error, { id }) => [
		{ type: 'User', id: 'LIST' },
		{ type: 'User', id },
	  ],    
	}),
    
    beginResetPassword: builder.mutation({
      query: (params) => ({
        url: 'users/reset-password',
        method: 'POST',
        params,
      }),
    }),

    beginChangeEmail: builder.mutation({
      query: (params) => ({
        url: 'users/change-email',
        method: 'POST',
        params,
      }),
    }),

    deleteUser: builder.mutation({
      query: (id) => ({
        url: `/users/delete/${id}`,
        method: 'POST',
      }),
      invalidatesTags: [{ type: 'User', id: 'LIST' }],
    }),
  }),
});

export const {
  useGetUsersQuery,
  useGetUserQuery,
  useGetUserTransactionsQuery,
  useUpdateUserMutation,
  useBeginResetPasswordMutation,
  useBeginChangeEmailMutation,
  useDeleteUserMutation,
} = userEndpoints;
