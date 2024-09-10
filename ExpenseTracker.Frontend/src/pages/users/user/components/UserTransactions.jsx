import React, { useState } from 'react';
import { useGetUserTransactionsQuery } from '../../../../state/user/api';
import { Button, Group, Skeleton, Stack, Table, Text, Title } from '@mantine/core';

const UserTransactions = ({ id }) => {
	const [pageNumber, setPageNumber] = useState(1);
	const [pageSize, setPageSize] = useState(5);

	const { data: userTransactions = {}, isLoading, isError } = useGetUserTransactionsQuery({ id, pageNumber, pageSize });

	if (isLoading || isError) {
		return <Skeleton h={300} w={'100%'} />;
	}

	return (
		<>
			<Stack justify='start' align='between'>
				<Table
					striped
					highlightOnHover
					withTableBorder
					style={{
						height: '220px',
					}}
				>
					<Table.Thead>
						<Table.Th>Details</Table.Th>
						<Table.Th>Category</Table.Th>
						<Table.Th>Date</Table.Th>
						<Table.Th>Amount</Table.Th>
						<Table.Th>Type</Table.Th>
					</Table.Thead>
					<Table.Tbody>
						{userTransactions.items?.map((t) => {
							return (
								<Table.Tr className='cursor-pointer' key={t.id}>
									<Table.Td>{t.description}</Table.Td>
									<Table.Td>{t.category.name}</Table.Td>

									<Table.Td>{new Date(t.date).toLocaleDateString('en-US')}</Table.Td>

									<Table.Td>RON {t.amount}</Table.Td>

									<Table.Td>{t.transactionType}</Table.Td>
								</Table.Tr>
							);
						})}
					</Table.Tbody>
				</Table>
				<Group
					align='center'
					justify='center'
					style={{
						width: '100%',
					}}
				>
					{userTransactions.pageNumbers?.map((p) => {
						return <Button onClick={() => setPageNumber(p)}>{p}</Button>;
					})}
				</Group>
			</Stack>
		</>
	);
};

export default UserTransactions;
