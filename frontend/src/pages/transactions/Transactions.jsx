import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { notifications } from '@mantine/notifications';
import { ActionIcon, Button, Group, Pagination, Table, Title } from '@mantine/core';
import { IconCheck, IconEdit, IconTrash, IconX } from '@tabler/icons-react';
import { useDeleteTransactionMutation, useGetTransactionsQuery } from '../../state/transaction/api';

const LIMIT = 10;

const Transactions = () => {
	const [activePage, setPage] = useState(1);

	const { data: transactions = [], isLoading: isLoadingTransactions } = useGetTransactionsQuery({
		offset: (activePage - 1) * LIMIT,
		limit: LIMIT,
	});

	const [deleteTransaction] = useDeleteTransactionMutation();

	const handleDeleteTransaction = (categoryId) => async () => {
		await deleteTransaction(categoryId);
		notifications.show({
			title: 'Transaction Deleted',
			position: 'bottom-right',
		});
	};

	const columns = ['Id', 'Amount', 'Currency', 'Exchange Rate', 'Recurrent', 'Type', 'Description', 'Date', 'Actions'];

	return (
		<div>
			<Group justify='space-between'>
				<Title>Transactions</Title>
				<Link to='/transactions/new'>
					<Button>Add</Button>
				</Link>
			</Group>
			{isLoadingTransactions && <p>loading...</p>}
			<Table striped highlightOnHover>
				<Table.Thead>
					<Table.Tr>
						{columns.map((column) => (
							<Table.Th>{column}</Table.Th>
						))}
					</Table.Tr>
				</Table.Thead>
				<Table.Tbody>
					{transactions.map((transaction, transactionIndex) => (
						<Table.Tr key={transaction.id}>
							<Table.Td>{transactionIndex + 1}</Table.Td>
							<Table.Td>{transaction.amountToCurrency}</Table.Td>
							<Table.Td>{transaction.currency}</Table.Td>
							<Table.Td>{transaction.exchangeRate}</Table.Td>
							<Table.Td>{transaction.isRecurrent ? <IconCheck /> : <IconX />}</Table.Td>
							<Table.Td>{transaction.transactionType}</Table.Td>
							<Table.Td>{transaction.description}</Table.Td>
							<Table.Td>{transaction.date}</Table.Td>
							<Table.Td>
								<Link to={`/transactions/${transaction.id}`}>
									<ActionIcon>
										<IconEdit size={16} />
									</ActionIcon>
								</Link>
								<ActionIcon onClick={handleDeleteTransaction(transaction.id)}>
									<IconTrash size={16} />
								</ActionIcon>
							</Table.Td>
						</Table.Tr>
					))}
				</Table.Tbody>
			</Table>
			<Group justify='center' mt='lg'>
				<Pagination value={activePage} onChange={setPage} total={2} />
			</Group>
		</div>
	);
};

export default Transactions;
