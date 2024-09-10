import api from '../emptySplitApi';

export const transactionEndpoints = api.injectEndpoints({
  endpoints: (builder) => ({
    getTransactions: builder.query({
      query: (params) => ({
        url: '/transactions/list',
        method: 'GET',
        params,
      }),
      transformResponse: (res) => res,
      providesTags: (result) =>
        result
          ? [
              ...result.items.map(({ id }) => ({ type: 'Transaction', id })),
              { type: 'Transaction', id: 'LIST' },
            ]
          : [{ type: 'Transaction', id: 'LIST' }],
    }),
    getTransaction: builder.query({
      query: (id) => ({
        url: `/transactions/${id}`,
      }),
      transformResponse: (res) => res,
      providesTags: (result) => [{ type: 'Transaction', id: result?.data?.id }],
    }),
    createTransaction: builder.mutation({
      query: (body) => ({
        url: '/transactions/create',
        method: 'POST',
        body,
      }),
      invalidatesTags: [{ type: 'UserTransactions', id: 'USER-TRANSACTIONS' }],
    }),
    updateTransaction: builder.mutation({
      query: ({ id, ...data }) => ({
        url: `/transactions/update/${id}`,
        method: 'POST',
        body: data,
      }),
      invalidatesTags: (_result, _error, { id }) => [
        { type: 'Transaction', id: 'LIST' },
        { type: 'Transaction', id },
      ],
    }),
    deleteTransaction: builder.mutation({
      query: (id) => ({
        url: `/transactions/delete/${id}`,
        method: 'POST',
      }),
      invalidatesTags: [{ type: 'Transaction', id: 'LIST' }],
    }),
  }),
});

export const {
  useGetTransactionsQuery,
  useGetTransactionQuery,
  useCreateTransactionMutation,
  useUpdateTransactionMutation,
  useDeleteTransactionMutation,
} = transactionEndpoints;
