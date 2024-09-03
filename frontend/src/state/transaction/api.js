import api from '../emptySplitApi';

export const transactionEndpoints = api.enhanceEndpoints({ addTagTypes: ['Transaction'] }).injectEndpoints({
	endpoints: (builder) => ({
		getTransactions: builder.query({
			query: (params) => ({ url: '/transactions/list', params }),
			transformResponse: (res) =>
				res.map((transaction) => ({ ...transaction, amountToCurrency: transaction.amount / transaction.exchangeRate })),
			providesTags: (result) =>
				result
					? [...result.map(({ id }) => ({ type: 'Transaction', id })), { type: 'Transaction', id: 'LIST' }]
					: [{ type: 'Transaction', id: 'LIST' }],
		}),
		getTransaction: builder.query({
			query: (id) => ({ url: `/transactions/${id}` }),
			transformResponse: (res) => res,
			providesTags: (result) => [{ type: 'Transaction', id: result?.id }],
		}),
		createTransaction: builder.mutation({
			query: (body) => ({
				url: '/transactions',
				method: 'POST',
				body,
			}),
			invalidatesTags: [{ type: 'Transaction', id: 'LIST' }],
		}),
		updateTransaction: builder.mutation({
			query: (data) => ({
				url: `/transactions/${data.id}`,
				method: 'PUT',
				body: data,
			}),
			invalidatesTags: (_result, _error, { id }) => [
				{ type: 'Transaction', id: 'LIST' },
				{ type: 'Transaction', id },
			],
		}),
		deleteTransaction: builder.mutation({
			query: (id) => ({
				url: `/transactions/${id}`,
				method: 'DELETE',
			}),
			invalidatesTags: [{ type: 'Transaction', id: 'LIST' }],
		}),
	}),
});

export const {
	useGetTransactionsQuery,
	useGetTransactionQuery,
	useUpdateTransactionMutation,
	useCreateTransactionMutation,
	useDeleteTransactionMutation,
} = transactionEndpoints;
