import { Box, LoadingOverlay, Portal } from '@mantine/core';

export default function Loading({ visible = true }) {
	if (!visible) {
		return null;
	}

	return (
		<Portal>
			<Box
				style={{
					position: 'fixed',
					top: 0,
					left: 0,
					width: '100%',
					height: '100%',
					zIndex: 9999,
				}}
			>
				<Box style={{ position: 'relative', height: '100%' }}>
					<LoadingOverlay visible />
				</Box>
			</Box>
		</Portal>
	);
}
