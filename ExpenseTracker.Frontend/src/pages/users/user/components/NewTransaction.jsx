import { Button, Chip, Fieldset, Group, Overlay, Select, Stack, TextInput } from '@mantine/core';
import { DateTimePicker } from '@mantine/dates';
import React, { useState } from 'react';
import CreateTransactionOverlay from './CreateTransactionOverlay';

const NewTransaction = ({ id }) => {
	const [showOverlay, setShowOverlay] = useState(false);
	const handleCloseOverlay = () => {
		setShowOverlay(false);
	};
	return (
		<>
			<Button
				onClick={() => setShowOverlay(true)}
				style={{
					marginInlineStart: '100%',
				}}
			>
				Add Transaction
			</Button>
			{showOverlay && <CreateTransactionOverlay id={id} close={handleCloseOverlay} />}
		</>
	);
};

export default NewTransaction;
