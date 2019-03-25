import React from 'react';
import styled from 'styled-components'
import { TextField, Dialog, DialogActions, DialogContent, DialogTitle, Button, Paper } from '@material-ui/core';

import { connectTo } from '../../utils/generic'
import * as actions from '../../actions/editor'
import { TAGS_LIMIT } from '../../constants/editor';
import Tag from '../tag'

const InputLine = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
`

const ChipsContainer = styled(Paper)`
  && {
    margin: 10px 0;
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    justify-content: center;
  }
`

export default connectTo(
  state => state.editor,
  actions,
  ({ editTag, submitTag, deleteTag, tags, editingTag, toggleTagsMenu, publish }) => (
    <Dialog
      open={true}
      onClose={toggleTagsMenu}
      aria-labelledby="form-dialog-tags"
    >
      <DialogTitle id="form-dialog-tags">
       Добавьте или измените закладку (до {TAGS_LIMIT} штук) так чтобы читатель знал о чем она
      </DialogTitle>
      <DialogContent>
        {tags.length < 5 && (
          <InputLine>
            <TextField
              autoFocus
              margin="dense"
              label="Закладка"
              type="text"
              fullWidth
              value={editingTag}
              onChange={({ target: { value } }) => editTag(value)}
            />
            <Button onClick={submitTag} size='small' variant='outlined' color='primary'>
              Добавить
            </Button>
          </InputLine>
        )}
        <ChipsContainer>
          {
            tags.map(tag => (
              <Tag
                key={tag}
                label={tag}
                onDelete={() => deleteTag(tag)}
              />
            ))
          }
        </ChipsContainer>
      </DialogContent>
      <DialogActions>
        <Button style={{ margin: 'auto' }} color='primary' variant='contained' onClick={publish}>
          Опубликовать
        </Button>
      </DialogActions>
    </Dialog>
  )
)